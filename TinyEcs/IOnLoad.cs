using System;
using System.Collections.Generic;
using System.Text;

namespace TinyEcs
{
    /// <summary>
    /// Allows a dependency to have a <c>OnLoad</c> method that gets called after a <see cref="World"/> has been constructed 
    /// for further initialization dependant on a fully constructed <see cref="World"/>.
    /// </summary>
    public interface IOnLoad
    {
        /// <summary>
        /// Gets called after a <see cref="World"/> has been constructed
        /// </summary>
        /// <param name="world">The <see cref="World"/> that has been constructed</param>
        void OnLoad(World world);
    }
}
