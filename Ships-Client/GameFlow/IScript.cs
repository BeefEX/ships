using System;

namespace Ships_Client {
    public interface IScript {

        void Start();

        void Unload();

        void Update();

        void KeyPressed(ConsoleKeyInfo key);

    }
}