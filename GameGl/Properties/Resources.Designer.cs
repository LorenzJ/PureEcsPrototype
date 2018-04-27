﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GameGl.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("GameGl.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///uniform float uTime;
        ///
        ///in vec2 Position;
        ///
        ///out vec4 FragColor;
        ///
        ///void main()
        ///{
        ///    vec2 uv = Position;
        ///	uv *= .5;
        ///	uv.x *= 2.8;
        ///    uv.y += uTime * 0.2;
        ///    uv.x = mod(sin(uv.y), cos(uv.x));
        ///    uv.y = mod(-sin(uv.x), -cos(uv.y));
        ///    float mask1 = smoothstep(-0.50, -0.495, uv.x - uv.y * 0.2);
        ///    mask1 *= smoothstep(0.50, 0.495, uv.x + uv.y * 0.2);
        ///    mask1 -= smoothstep(0.40, 0.395, length(uv - vec2(0, 0.8)));
        ///    mask1 -= smoothstep(0.49, 0.495, uv.y);
        ///    mask1 -= sm [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BackgroundFrag {
            get {
                return ResourceManager.GetString("BackgroundFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec2 aPosition;
        ///
        ///out vec2 Position;
        ///
        ///void main() {
        ///	gl_Position = vec4(aPosition * 2., 0., 1.);
        ///	Position = aPosition;
        ///}.
        /// </summary>
        internal static string BackgroundVertex {
            get {
                return ResourceManager.GetString("BackgroundVertex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///uniform float uTime;
        ///
        ///in vec2 Position;
        ///
        ///out vec4 FragColor;
        ///
        ///void main()
        ///{
        ///	float t = uTime * 2.0;
        ///    vec2 uv = Position;
        ///
        ///    float circle = smoothstep(0.50, 0.49, length(uv));
        ///	float alpha = circle;
        ///	circle -= smoothstep(0.45, 0.44, length(uv));
        ///    circle += smoothstep(0.4, 0.39, length(uv));
        ///    
        ///    vec2 orig = uv;
        ///    uv.x += cos(t) * 0.2;
        ///    uv.y += sin(t) * 0.2;
        ///    circle -= smoothstep(0.1, 0.09, length(uv));
        ///    uv = orig;
        ///    uv.x -= cos(t) * 0.2;
        ///    [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string BulletFrag {
            get {
                return ResourceManager.GetString("BulletFrag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout(location = 0) in vec2 aPosition;
        ///layout(location = 1) in vec2 aOffset;
        ///
        ///out vec2 Position;
        ///
        ///void main()
        ///{
        ///	gl_Position = vec4(aPosition * 0.1 + aOffset, 0, 1);
        ///	Position = aPosition;
        ///}.
        /// </summary>
        internal static string BulletVertex {
            get {
                return ResourceManager.GetString("BulletVertex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to #version 330 core
        ///
        ///layout (location = 0) in vec2 aPosition;
        ///
        ///void main() {
        ///	gl_Position = vec4(aPosition, 0., 1.);
        ///}.
        /// </summary>
        internal static string SimpleVertex {
            get {
                return ResourceManager.GetString("SimpleVertex", resourceCulture);
            }
        }
    }
}
