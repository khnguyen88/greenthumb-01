#ifndef TASK_TIMER_H
#define TASK_TIMER_H

#ifdef abs
#undef abs
#endif
#include <chrono>

//DECLARATION OF ABSTRACT TASK TIMER CLASS
template <typename T>
class TaskTimer {
    protected:
        // Properties
        unsigned long currentDurationMillis;
        unsigned long currentTimeMillis;
        unsigned long startTimeMillis;
        unsigned long maxPeriodTimeMillis;

        // Default Destructor
        virtual ~TaskTimer() = default;

    private:
        // Methods
        virtual void setCurrentTime() = 0;
        virtual void setCurrentDuration() = 0;
        virtual void refreshTimer(bool isPassedDuration) = 0;

    public:
        // Properties
        bool isItTaskTime;

        // Default Constructor
        TaskTimer() = default;

        // Methods
        virtual void syncStartTime(T otherStartTimeMillis) = 0;
        virtual void setMaxPeriod(T maxPerMillis) = 0;
        virtual bool checkIsItTaskTime() = 0;

        virtual T getStartTimeMillis() = 0;
        virtual T getMaxPeriodTimeMillis() = 0;
        virtual T getCurrentTimeMillis() = 0;
        virtual T getCurrentDuration() = 0;
};

#endif