using System;
using System.Collections.Generic;

namespace Utility {
    public class PriorityQueue<T> {
        private readonly List<(T item, float priority)> items = new();

        public int Count => items.Count;

        public void Enqueue(T item, float priority) {
            items.Add((item, priority));
        }

        public bool TryDequeue(out T item) {
            if (items.Count == 0) {
                item = default;
                return false;
            }

            item = Dequeue();
            return true;
        }

        public T Dequeue() {
            if (items.Count == 0) {
                throw new InvalidOperationException("Cannot dequeue from an empty PriorityQueue.");
            }

            int bestIndex = 0;

            for (int i = 1; i < items.Count; i++) {
                if (items[i].priority < items[bestIndex].priority) {
                    bestIndex = i;
                }
            }

            T bestItem = items[bestIndex].item;
            items.RemoveAt(bestIndex);
            return bestItem;
        }
    }
}
