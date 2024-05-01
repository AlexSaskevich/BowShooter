using System.Collections.ObjectModel;
using UnityEngine.Scripting;

namespace Source.Scripts.BotEntity.Infrastructure.Containers
{
    public class Container<T>
    {
        [Preserve]
        public Container()
        {
        }

        public Container(ReadOnlyCollection<T> items)
        {
            Items = items;
        }

        public ReadOnlyCollection<T> Items { get; private set; }
    }
}