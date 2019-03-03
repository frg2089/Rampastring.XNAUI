﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rampastring.XNAUI
{
    /// <summary>
    /// Handles render targets.
    /// </summary>
    internal static class RenderTargetStack
    {
        public static void Initialize(RenderTarget2D finalRenderTarget, GraphicsDevice _graphicsDevice)
        {
            FinalRenderTarget = finalRenderTarget;
            graphicsDevice = _graphicsDevice;
            currentContext = new RenderContext(finalRenderTarget, null);
        }

        public static RenderTarget2D FinalRenderTarget { get; internal set; }

        private static RenderContext currentContext;

        private static GraphicsDevice graphicsDevice;

        public static void PushRenderTarget(RenderTarget2D renderTarget)
        {
            Renderer.EndDraw();
            RenderContext context = new RenderContext(renderTarget, currentContext);
            currentContext = context;
            graphicsDevice.SetRenderTarget(renderTarget);
            Renderer.BeginDraw();
        }

        public static void PopRenderTarget()
        {
            currentContext = currentContext.PreviousContext;

            if (currentContext == null)
            {
                throw new InvalidOperationException("No render context left! This usually " +
                    "indicates that a control with an unique render target has " +
                    "double-popped their render target.");
            }

            Renderer.EndDraw();
            graphicsDevice.SetRenderTarget(currentContext.RenderTarget);
            Renderer.BeginDraw();
        }
    }

    internal class RenderContext
    {
        public RenderContext(RenderTarget2D renderTarget, RenderContext previousContext)
        {
            RenderTarget = renderTarget;
            PreviousContext = previousContext;
        }

        public RenderTarget2D RenderTarget { get; }
        public RenderContext PreviousContext { get; }
    }
}