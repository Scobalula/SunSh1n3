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
using System.IO;
using System.Text;

namespace SunSh1n3.IO
{
    /// <summary>
    /// Extensions for <see cref="BinaryReader"/>
    /// </summary>
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Reads a null terminated string from the <see cref="BinaryReader"/>
        /// </summary>
        /// <param name="reader">Reader to read from</param>
        /// <returns>Resulting String</returns>
        public static string ReadNullTerminatedString(this BinaryReader reader)
        {
            var builder = new StringBuilder(256);

            while (true)
            {
                var c = reader.ReadByte();
                if (c == 0)
                    break;
                builder.Append((char)c);
            }

            return builder.ToString();
        }
    }
}
