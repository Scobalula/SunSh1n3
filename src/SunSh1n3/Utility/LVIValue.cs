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

namespace SunSh1n3.Utility
{
    /// <summary>
    /// A class to hold an LVI Value
    /// </summary>
    public class LVIValue : UIItem
    {
        /// <summary>
        /// Gets or Sets the Name of the Value
        /// </summary>
        public string Name
        {
            get => GetValue<string>("Name");
            set => SetValue(value, "Name");
        }

        /// <summary>
        /// Gets or Sets the X Component
        /// </summary>
        public float X
        {
            get => GetValue<float>("X");
            set => SetValue(value, "X");
        }

        /// <summary>
        /// Gets or Sets the Y Component
        /// </summary>
        public float Y
        {
            get => GetValue<float>("Y");
            set => SetValue(value, "Y");
        }

        /// <summary>
        /// Gets or Sets the Z Component
        /// </summary>
        public float Z
        {
            get => GetValue<float>("Z");
            set => SetValue(value, "Z");
        }

        /// <summary>
        /// Gets or Sets the W Component
        /// </summary>
        public float W
        {
            get => GetValue<float>("W");
            set => SetValue(value, "W");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LVIValue"/> class
        /// </summary>
        /// <param name="name">Value Name</param>
        public LVIValue(string name)
        {
            Name = name;
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
            W = 0.0f;
        }

        /// <summary>
        /// Resets the <see cref="LVIValue"/>
        /// </summary>
        public void Reset()
        {
            X = 0.0f;
            Y = 0.0f;
            Z = 0.0f;
            W = 0.0f;
        }
    }
}
