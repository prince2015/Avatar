﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:2.0.50727.4927
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace BreakOut {
    using System;
    
    
    /// <summary>
    ///   强类型资源类，用于查找本地化字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resource1 {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resource1() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("BreakOut.Resource1", typeof(Resource1).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   为使用此强类型资源类的所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
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
        
        internal static System.Drawing.Bitmap ball {
            get {
                object obj = ResourceManager.GetObject("ball", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.IO.UnmanagedMemoryStream brick {
            get {
                return ResourceManager.GetStream("brick", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap CactusBG {
            get {
                object obj = ResourceManager.GetObject("CactusBG", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap desertBG {
            get {
                object obj = ResourceManager.GetObject("desertBG", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.IO.UnmanagedMemoryStream laser {
            get {
                return ResourceManager.GetStream("laser", resourceCulture);
            }
        }
        
        internal static System.IO.UnmanagedMemoryStream laserslow {
            get {
                return ResourceManager.GetStream("laserslow", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap orangeBrick {
            get {
                object obj = ResourceManager.GetObject("orangeBrick", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.IO.UnmanagedMemoryStream paddle {
            get {
                return ResourceManager.GetStream("paddle", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap purpleBrick {
            get {
                object obj = ResourceManager.GetObject("purpleBrick", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.IO.UnmanagedMemoryStream realslowlaser {
            get {
                return ResourceManager.GetStream("realslowlaser", resourceCulture);
            }
        }
        
        internal static System.IO.UnmanagedMemoryStream start {
            get {
                return ResourceManager.GetStream("start", resourceCulture);
            }
        }
        
        internal static System.Drawing.Bitmap starter {
            get {
                object obj = ResourceManager.GetObject("starter", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap stoneBrick {
            get {
                object obj = ResourceManager.GetObject("stoneBrick", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.Drawing.Bitmap tanBrick {
            get {
                object obj = ResourceManager.GetObject("tanBrick", resourceCulture);
                return ((System.Drawing.Bitmap)(obj));
            }
        }
        
        internal static System.IO.UnmanagedMemoryStream wall {
            get {
                return ResourceManager.GetStream("wall", resourceCulture);
            }
        }
    }
}
