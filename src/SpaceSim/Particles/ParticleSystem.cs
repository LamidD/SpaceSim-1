﻿using System.Collections.Generic;
using System.Drawing;
using SpaceSim.Drawing;
using VectorMath;

namespace SpaceSim.Particles
{
    abstract class ParticleSystem
    {
        protected Particle[] _particles;
        protected Queue<int> _availableParticles;

        private Color _color;

        protected ParticleSystem(int particleCount, Color color)
        {
            _particles = new Particle[particleCount];

            _availableParticles = new Queue<int>(particleCount);

            for (int i = 0; i < particleCount; i++)
            {
                _particles[i] = new Particle();

                _availableParticles.Enqueue(i);
            }

            _color = color;
        }

        public void Draw(Graphics graphics, Camera camera)
        {
            var particleBounds = new List<RectangleF>();

            float particleScale;

            // Scale particle size with viewport width
            if (camera.Bounds.Width > 1000)
            {
                particleScale = 1.5f;
            }
            else
            {
                particleScale = (float)(1.22e-6 * camera.Bounds.Width * camera.Bounds.Width - 4.8e-3 * camera.Bounds.Width + 5.5);
            }

            float halfParticleScale = particleScale * 0.5f;

            foreach (Particle particle in _particles)
            {
                if (particle.IsActive)
                {
                    if (camera.Contains(particle.Position))
                    {
                        PointF localPoint = RenderUtils.WorldToScreen(particle.Position, camera.Bounds);

                        particleBounds.Add(new RectangleF(localPoint.X - halfParticleScale,
                                                          localPoint.Y - halfParticleScale,
                                                          particleScale, particleScale));
                    }
                }
            }

            camera.ApplyScreenRotation(graphics);

            RenderUtils.DrawRectangles(graphics, particleBounds, _color);

            graphics.ResetTransform();
        }
    }
}