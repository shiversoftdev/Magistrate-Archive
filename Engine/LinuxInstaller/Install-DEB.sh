#!/bin/bash

if [ "$EUID" -ne 0 ]
  then echo "Please run as root"
  exit
fi

#wget -q https://packages.microsoft.com/config/ubuntu/16.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
#wait
#sudo dpkg -i packages-microsoft-prod.deb
#wait
#sudo apt-get update
#sudo apt-get install apt-transport-https
#sudo apt-get update
#sudo apt-get install dotnet-sdk-3.0
#wait

sudo apt install curl

#sudo mkdir /usr/sbin

#stat /usr/sbin

sudo curl -sSL https://dot.net/v1/dotnet-install.sh | sudo bash /dev/stdin -Channel 3.0 -InstallDir /usr/sbin
wait

sudo dotnet LinuxInstaller.dll