using System;
using System.Collections.Generic;

namespace Game.Actors
{
    public class ActorBlock
    {
        private readonly Dictionary<InputBlock, int> _blockCount = new();

        private readonly InputBlock[] _flags = Enum.GetValues(typeof(InputBlock)) as InputBlock[];

        public bool HasAny(InputBlock input)
        {
            foreach (InputBlock flag in _flags)
            {
                if (!input.HasFlagFast(flag)) 
                    continue;
                
                if (_blockCount.ContainsKey(flag) && !IsBlockEmpty(flag))
                    return true;
            }

            return false;
        }

        public void SetBlock(InputBlock toBlock)
        {
            foreach (InputBlock flag in _flags)
            {
                if (toBlock.HasFlagFast(flag))
                    SetBlockInternal(flag);
            }
        }

        public void RemoveBlock(InputBlock toRemove)
        {
            foreach (InputBlock flag in _flags)
            {
                if (toRemove.HasFlagFast(flag))
                    RemoveBlockInternal(flag);
            }
        }

        public void Reset()
            => _blockCount.Clear();

        private void SetBlockInternal(InputBlock toBlock)
        {
            if (_blockCount.ContainsKey(toBlock))
                _blockCount[toBlock]++;
            else
                _blockCount.Add(toBlock, 1);
        }

        private void RemoveBlockInternal(InputBlock flag)
        {
            if (!_blockCount.ContainsKey(flag))
                return;

            _blockCount[flag]--;

            if (IsBlockEmpty(flag))
                _blockCount.Remove(flag);
        }

        private bool IsBlockEmpty(InputBlock flag)
            => _blockCount[flag] == 0;
    }
}