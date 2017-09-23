namespace Ships_Client {
    
    public class Scene {

        public readonly string name;

        public readonly IScript script;

        public Scene(string name, IScript script) {
            this.name = name;
            this.script = script;
        }
    }
}