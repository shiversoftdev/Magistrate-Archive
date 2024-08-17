#!/bin/bash

### BEGIN INIT INFO
# Provides:          dns.sh
# Required-Start:    $remote_fs
# Required-Stop:     $remote_fs
# Should-Start:      $network $syslog
# Should-Stop:       $network $syslog
# Default-Start:     5
# Default-Stop:      0 1 2 3 4 6
# Short-Description: Start and stop vmware dnsfix
# Description:       dns.sh is a quickfix for a nat issue with bind
### END INIT INFO

systemctl stop NetworkManager
systemctl disable NetworkManager

# Replace the static line and force DHCP for eth0
old="$(cat /etc/network/interfaces | grep "iface eth0")"
new="iface eth0 inet dhcp"
sed -i "/$old/ c\\$new" /etc/network/interfaces

systemctl restart networking

# Replace the ip address in the file with our dhcp first 3 octets and 111 allowed via documentation
ip="$(ifconfig eth0 | egrep -o "inet addr:(([0-9]{1,3}\.){3}[0-9]{1,3})" | cut -d':' -f2 | egrep -o "\b([0-9]{1,3}\.){3}")111"
old="$(cat /etc/network/interfaces | grep address)"
sed -i "/$old/ c\\	address $ip" /etc/network/interfaces

# Replace the netmask with /24 allowed via documentation
old="$(cat /etc/network/interfaces | grep netmask)"
sed -i "/$old/ c\\	netmask 255.255.255.0" /etc/network/interfaces

# Replace the gateway with the first 3 octals of our dhcp address and 2 allowed via documentation
old="$(cat /etc/network/interfaces | grep gateway)"
new="$(echo $ip | egrep -o "\b([0-9]{1,3}\.){3}")2"
sed -i "/$old/ c\\	gateway $new" /etc/network/interfaces

# Replace the networking configuration to allow a static ip configuration
old="$(cat /etc/network/interfaces | grep "iface eth0")"
new="iface eth0 inet static"
sed -i "/$old/ c\\$new" /etc/network/interfaces

# Replace static ip in reverse zone /etc/bind/zones/db.static-ip
old="$(cat /etc/bind/zones/db.static-ip | egrep "^ns1.ussf.gov" | egrep -o "([0-9]{1,3}[\.]){3}[0-9]{1,3}")"
new="ns1.ussf.gov	IN	A	$ip ; DEVELOPER NOTE: DO NOT EDIT THIS LINE"
sed -i "/$old/ c\\$new" /etc/bind/zones/db.static-ip

# /etc/bind/zones/db.ussf.gov
old="$(cat /etc/bind/zones/db.ussf.gov | egrep -o "([0-9]{1,3}[\.]){3}[0-9]{1,3}")"

for line in $old; do
	sed -i "s/$line/$ip/g" /etc/bind/zones/db.ussf.gov
done

# Restart the bind service
systemctl restart bind9

# Refresh records cache
rndc reload

# Replace DHCP auto-resolve override
echo -e "#!/bin/sh\n" > /etc/dhcp/dhclient-enter-hooks.d/nodnsupdate
echo -e "make_resolv_conf() {\n" >> /etc/dhcp/dhclient-enter-hooks.d/nodnsupdate
echo -e "	:\n" >> /etc/dhcp/dhclient-enter-hooks.d/nodnsupdate
echo -e "}" >> /etc/dhcp/dhclient-enter-hooks.d/nodnsupdate
chmod +x /etc/dhcp/dhclient-enter-hooks.d/nodnsupdate

# Restart the networking service
systemctl restart networking

# set dns search to ussf.gov
echo "search ussf.gov" > /etc/resolv.conf

# set dns to static ip address
echo "nameserver $ip" >> /etc/resolv.conf

# Restart the networking service
systemctl restart networking