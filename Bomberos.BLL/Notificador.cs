using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bomberos.BLL {

    public class Notificador : INotifyPropertyChanged {
        private readonly object @lock = new object ( );

        private PropertyChangedEventHandler propertyChanged;

        public event PropertyChangedEventHandler PropertyChanged {
            add {
                lock (@lock) {
                    this.propertyChanged += value;
                }
            }
            remove {
                lock (@lock) {
                    this.propertyChanged -= value;
                }
            }
        }

        protected void OnPropertyChanged ([CallerMemberName] String propertyName = "") {
            var handler = null as PropertyChangedEventHandler;

            lock (@lock) {
                handler = this.propertyChanged;
            }

            handler?.Invoke (this, new PropertyChangedEventArgs (propertyName));
        }

        public void Liberar ( ) {
            lock (@lock) {
                this.propertyChanged = null;
            }
        }
    }
}