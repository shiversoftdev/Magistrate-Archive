#!/bin/bash

### BEGIN INIT INFO
# Provides:          compscoring.sh
# Required-Start:    $remote_fs
# Required-Stop:     $remote_fs
# Should-Start:      $network $syslog
# Should-Stop:       $network $syslog
# Default-Start:     5
# Default-Stop:      0 1 2 3 4 6
# Short-Description: Competition Scoring Service
# Description:       Do not edit. Will break image.
### END INIT INFO

dotnet /ss-scoring/Engine/LinuxEngine.dll &