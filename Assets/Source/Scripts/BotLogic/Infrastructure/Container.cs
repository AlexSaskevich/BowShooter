using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Source.Scripts.BotLogic.Infrastructure
{
    public class Container<T>
    {
        [Preserve]
        public Container()
        {
        }

        public Container(IReadOnlyCollection<T> items)
        {
            Items = items;
        }

        public IReadOnlyCollection<T> Items { get; private set; }
    }
}