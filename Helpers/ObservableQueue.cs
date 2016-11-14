using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaHotHManager.Helpers
{
    public class ObservableQueue<T> : Collection<T>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        private Queue<T> internalQueue = new Queue<T>();

        public void Enqueue(T item)
        {
            internalQueue.Enqueue(item);
            onCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
            onPropertyChanged(this, nameof(internalQueue));
        }

        public T Dequeue()
        {
            var item = internalQueue.Dequeue();
            onCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
            onPropertyChanged(this, nameof(internalQueue));
            return item;
        }

        public T Peek()
        {
            return internalQueue.Peek();
        }

        public void CopyTo(Array to, int length)
        {
            internalQueue.CopyTo((T[])to, length);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(object sender, string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void onCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
    }
}
