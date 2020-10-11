using NvAPIWrapper;
using NvAPIWrapper.GPU;
using NvAPIWrapper.Native.GPU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanCtrl
{
    public class NvAPIFanControl : BaseControl
    {
        public delegate void OnSetNvAPIControlHandler(int index, int coolerID, int value, CoolerPolicy policy);
        public event OnSetNvAPIControlHandler onSetNvAPIControlHandler;

        private int mIndex = 0;
        private int mCoolerID = 0;
        private int mMinSpeed = 0;
        private int mMaxSpeed = 100;
        private CoolerPolicy mDefaultPolicy;
        private CoolerPolicy mCurrentPolicy;

        public NvAPIFanControl(string name, int index, int coolerID, int value, int minSpeed, int maxSpeed, CoolerPolicy defaultPolicy) : base()
        {
            Name = name;
            mIndex = index;
            mCoolerID = coolerID;
            Value = value;
            LastValue = value;
            mMinSpeed = minSpeed;
            mMaxSpeed = maxSpeed;
            mDefaultPolicy = defaultPolicy;
        }

        public override void update()
        {

        }

        public override void stop()
        {
            mCurrentPolicy = mDefaultPolicy;
            onSetNvAPIControlHandler(mIndex, mCoolerID, Value, mDefaultPolicy);
        }

        public override int getMinSpeed()
        {
            return mMinSpeed;
        }

        public override int getMaxSpeed()
        {
            return mMaxSpeed;
        }

        public override int setSpeed(int value)
        {
            if (value > mMaxSpeed)
            {
                Value = mMaxSpeed;

                mCurrentPolicy = CoolerPolicy.Manual;
            }
            else if (value < mMinSpeed)
            {
                Value = mMinSpeed;

                mCurrentPolicy = mDefaultPolicy;
            }
            else
            {
                Value = value;

                mCurrentPolicy = CoolerPolicy.Manual;
            }

            onSetNvAPIControlHandler(mIndex, mCoolerID, Value, mCurrentPolicy);

            LastValue = Value;
            return Value;
        }
    }
}
