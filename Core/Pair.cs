using System;
using System.Collections.Generic;
using System.Text;

namespace BlackHen.Bridge
{
    /// <summary>
    ///   The direction that a <see cref="Pair"/> is playing.
    /// </summary>
    public enum Partnership
    {
        NorthSouth = 0,
        EastWest = 1
    };

    public class Pair
    {
        /// <summary>
        ///   The number assigned to the <see cref="Pair"/> for the <see cref="Tournament"/>.
        /// </summary>
        /// <remarks>
        ///   This is usually the table number that the <see cref="Pair"/> are initially
        ///   seated at.  In most movements, the <see cref="Partnership.NorthSouth"/> partnership
        ///   remains at the table and <see cref="Partnership"/> moves around the room.
        /// </remarks>
        public int Number { get; set; }

        /// <summary>
        ///   The direction that the <see cref="Pair"/> is playing.
        /// </summary>
        public Partnership Partnership { get; set; }
    }
}
