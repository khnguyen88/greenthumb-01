#ifndef CHRONO_TASK_TIMER
#define CHRONO_TASK_TIMER

#define _CHRONO_TP std::chrono::time_point
#define _CHRONO_SC_D std::chrono::steady_clock::duration

#include <chrono>
#include "chrono_ext.cpp"
#include "task_timer.h"

//DECLARATION OF CHRONO TIMER CLASS

class ChronoTaskTimer : public TaskTimer<unsigned long> {
    private:
        // Properties
        _CHRONO_TP <std::chrono::steady_clock> startChronoTime;
        _CHRONO_SC_D startChronoDurationSinceEpoch;
        long long startChronoDurationSinceEpochInMillis;

        _CHRONO_TP <std::chrono::steady_clock> currentChronoTime;
        _CHRONO_SC_D currentChronoDurationSinceEpoch;
        long long currentChronoDurationSinceEpochInMillis;

        // Methods
        void setCurrentTime() override;
        void setCurrentDuration() override;
        void refreshTimer(bool isPassedDuration) override;

    public:
        // Default Constructor
        ChronoTaskTimer();
        
        // Public Delegating Constructor
        ChronoTaskTimer(unsigned long maxPerMillis);

        // Default Destructor
        ~ChronoTaskTimer() = default;

        
        // Methods
        void syncStartTime(unsigned long otherStartTimeMillis) override;
        void setMaxPeriod(unsigned long maxPerMillis) override;
        bool checkIsItTaskTime() override;

        unsigned long getStartTimeMillis() override;
        unsigned long getMaxPeriodTimeMillis() override;
        unsigned long getCurrentTimeMillis() override;
        unsigned long getCurrentDuration() override;

        
};

#endif