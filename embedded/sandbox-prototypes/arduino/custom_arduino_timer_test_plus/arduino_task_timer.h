#ifndef ARDUINO_TASK_TIMER
#define ARDUINO_TASK_TIMER

#include "task_timer.h"

//DECLARATION OF ARDUINO TIMER CLASS

class ArduinoTaskTimer : public TaskTimer<unsigned long> {
    private:
        // Methods
        void setCurrentTime() override;
        void setCurrentDuration() override;
        void refreshTimer(bool isPassedDuration) override;

    public:
        // Default Constructor
        ArduinoTaskTimer();
        
        // Public Delegating Constructor
        ArduinoTaskTimer(unsigned long maxPerMillis);

        // Default Destructor
        ~ArduinoTaskTimer() = default;

        
        // Methods
        void syncStartTime(unsigned long otherStartTimeMillis) override;
        void setMaxPeriod(unsigned long maxPerMillis) override;
        bool checkIsItTaskTime() override;
        void resetTaskConditions();

        unsigned long getStartTimeMillis() override;
        unsigned long getMaxPeriodTimeMillis() override;
        unsigned long getCurrentTimeMillis() override;
        unsigned long getCurrentDuration() override;

        
};

#endif