using System;
using Ships_Common;

namespace Ships_Client.Rendering {
    public class Loader {


        public event OnLoaderFinishHandler OnLoaderFinish;
        public delegate void OnLoaderFinishHandler();
        
        public bool showLoader = true;
        private int index;
        private int counter;

        private Vector2 position;
        
        public Loader(Vector2 position) {
            Reset();
            this.position = position;
        }

        public void Reset() {
            counter = 0;
            index = 0;
        }
        
        public void Render() {
            counter++;
            counter = counter % 9;
            
            if (counter == 0) {
                index++;
                index = index % Renderer.loadingCircle.Length;

                if (showLoader)
                    Renderer.drawPatternAsPixel(position,
                        Renderer.loadingCircle[index]);
                else if (index == 0 && OnLoaderFinish != null) {
                    OnLoaderFinish.Invoke();
                }
            }
        }
    }
}