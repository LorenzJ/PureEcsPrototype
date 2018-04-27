﻿using System;

namespace GameGl.Core
{
    public struct BindLock<T> : IDisposable
        where T : IBindable
    {
        private T bindable;

        private BindLock(T bindable)
        {
            this.bindable = bindable;
        }

        public static BindLock<T> Bind(T bindable)
        {
            bindable.Bind();
            return new BindLock<T>(bindable);
        }

        public void Dispose()
        {
            bindable.Unbind();
        }
    }
}
