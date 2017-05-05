using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Makaretu.Bridge.Analysis
{
    /// <summary>
    ///   All the <see cref="Contract">contracts</see> that can be made for a
    ///   specific <see cref="Board"/>.
    /// </summary>
    public class DoubleDummySolution : List<Contract>
    {
        /// <summary>
        ///   Gets the contract that can be made by <see cref="Seat">declaror</see> for the specified
        ///   <see cref="Denomination"/>
        /// </summary>
        /// <param name="declaror">
        ///   The declaror's <see cref="Seat"/>.
        /// </param>
        /// <param name="denomination">
        ///   The <see cref="Denomination"/> (or strain) to make.
        /// </param>
        /// <returns>
        ///   A makeable <see cref="Contract"/> or <see cref="Contract.PassedIn"/> 
        ///   (<see cref="Contract.Level"/> equals zero).
        /// </returns>
        public Contract this[Seat declaror, Denomination denomination]
        {
            get
            {
                foreach (var contract in this)
                {
                    if (contract.Declaror == declaror && contract.Denomination == denomination)
                        return contract;
                }
                return Contract.PassedIn;
            }
        }
    }
}
