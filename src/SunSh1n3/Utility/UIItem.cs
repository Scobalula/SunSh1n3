// ------------------------------------------------------------------------
// SunSh1n3 - Call of Duty: Black Ops III LVI Editor
// Copyright(C) 2020 Philip/Scobaluila
// ------------------------------------------------------------------------
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// ------------------------------------------------------------------------
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.
// ------------------------------------------------------------------------
// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// ------------------------------------------------------------------------
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SunSh1n3.Utility
{
    /// <summary>
    /// A base class for UI Items
    /// </summary>
    public abstract class UIItem : Dictionary<string, object>, INotifyPropertyChanged
    {
        /// <summary>
        /// Property Changed Event Handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the value of the given property
        /// </summary>
        protected T GetValue<T>([CallerMemberName] string propertyName = "")
        {
            if (TryGetValue(propertyName, out var value))
                return (T)value;
            else
                return default(T);
        }

        /// <summary>
        /// Sets the value of the given property
        /// </summary>
        protected void SetValue<T>(T newValue, [CallerMemberName] string propertyName = "")
        {
            this[propertyName] = newValue;
            NotifyPropertyChanged(propertyName);
        }

        /// <summary>
        /// Notifies that the property has changed
        /// </summary>
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
