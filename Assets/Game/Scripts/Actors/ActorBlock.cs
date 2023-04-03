using System;
using System.Collections.Generic;

namespace Game.Actors
{
    public class ActorBlock
    {
        private readonly Dictionary<InputBlock, int> _blockCount = new();

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public bool IsBlocked(InputBlock blocked)
        {
            foreach (InputBlock flag in Enum.GetValues(typeof(InputBlock)))
            {
                if (!blocked.HasFlagFast(flag))
                    continue;

                if (!_blockCount.ContainsKey(flag) || _blockCount[flag] == 0)
                    return false;
            }

            return true;
        }

        public void SetBlock()
            => SetBlock(InputBlockExtensions.FULL_BLOCK);

        public void SetBlock(InputBlock toBlock)
        {
            foreach (InputBlock flag in Enum.GetValues(typeof(InputBlock)))
            {
                if (toBlock.HasFlagFast(flag))
                    SetBlockInternal(flag);
            }
        }

        public void RemoveBlock()
            => RemoveBlock(InputBlockExtensions.FULL_BLOCK);

        public void RemoveBlock(InputBlock toRemove)
        {
            foreach (InputBlock flag in Enum.GetValues(typeof(InputBlock)))
                if (toRemove.HasFlagFast(flag))
                    RemoveBlockInternal(flag);
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

            if (_blockCount[flag] == 0)
                _blockCount.Remove(flag);
        }
    }
}