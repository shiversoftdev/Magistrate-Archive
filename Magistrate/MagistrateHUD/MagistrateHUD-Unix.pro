QT       += core gui

greaterThan(QT_MAJOR_VERSION, 4): QT += widgets

QT += multimedia multimediawidgets

CONFIG += c++11

# The following define makes your compiler emit warnings if you use
# any Qt feature that has been marked deprecated (the exact warnings
# depend on your compiler). Please consult the documentation of the
# deprecated API in order to know how to port your code away from it.
DEFINES += QT_DEPRECATED_WARNINGS

# You can also make your code fail to compile if it uses deprecated APIs.
# In order to do so, uncomment the following line.
# You can also select to disable deprecated APIs only up to a certain version of Qt.
#DEFINES += QT_DISABLE_DEPRECATED_BEFORE=0x060000    # disables all the APIs deprecated before Qt 6.0.0

SOURCES += \
    MagistrateAudio.cpp \
    forensicsanswerbox.cpp \
    forensicsbox.cpp \
    forensicsinterface.cpp \
    guioptions.cpp \
    main.cpp \
    mainwindow.cpp \
    mnotifyform.cpp \
    scorelineitem.cpp \
    scoringreport.cpp

HEADERS += \
    CSSE_CONST.h \
    MagistrateAudio.h \
    ScoringTypes.h \
    VM_CONST.h \
    forensicsanswerbox.h \
    forensicsbox.h \
    forensicsinterface.h \
    guioptions.h \
    mainwindow.h \
    mnotifyform.h \
    scorelineitem.h \
    scoringreport.h

FORMS += \
    forensicsanswerbox.ui \
    forensicsbox.ui \
    forensicsinterface.ui \
    guioptions.ui \
    mainwindow.ui \
    mnotifyform.ui \
    scorelineitem.ui \
    scoringreport.ui

# Default rules for deployment.
qnx: target.path = /tmp/$${TARGET}/bin
else: unix:!android: target.path = /opt/$${TARGET}/bin
!isEmpty(target.path): INSTALLS += target

RESOURCES += \
    rc/QTRC.qrc
