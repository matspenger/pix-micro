declare var require: any

var React = require('react');
var ReactDOM = require('react-dom');

const serverAddr = "http://localhost:7071/api/";
const base64UriScheme = "data:image/jpeg;base64,";

interface IProps {

}

interface IState {
    imageBase64: string
}

export class App extends React.Component<IProps, IState> {

    constructor(props, state) {
        super(props, state);
        this.state = {
            imageBase64: ""
        };

        this.loadImage = this.loadImage.bind(this);
        this.flip = this.flip.bind(this);
    }

    flip(mode: string) {
        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = () => {
            if (xhr.readyState === 4 && xhr.status === 200) {
                let responseObj = JSON.parse(xhr.response);
                let base64 = responseObj["OutputImage"];
                if (!base64.startsWith(base64UriScheme)) {
                    base64 = base64UriScheme + base64;
                }
                this.setState({ imageBase64: base64 });
            }
        };
        let content = JSON.stringify({
            "Mode": mode,
            "InputImage": this.state.imageBase64
        });
        xhr.open("POST", serverAddr + "flip");
        xhr.setRequestHeader("Content-Type", "text/plain");
        xhr.send(content);
    }

    loadImage(event) {
        let fr = new FileReader();
        fr.addEventListener("load", () => {
            this.setState({ imageBase64: fr.result });
        })
        fr.readAsDataURL(event.target.files[0]);
    }

    render() {
        return (
            <div id="content">
                <div className="toolbar">
                    <button>Undo</button>
                    <button>Redo</button>
                    <button onClick={() => this.flip("X")}>Flip X</button>
                    <button onClick={() => this.flip("Y")}>Flip Y</button>
                    <button onClick={() => this.flip("XY")}>Flip XY</button>
                </div>
                <div id="image">
                    <label htmlFor="load-img">Load image file:</label>
                    <input type="file" id="load-img" name="load-img" onChange={this.loadImage} />
                    <img src={this.state.imageBase64} />
                </div>
            </div>
        );
    }
}

ReactDOM.render(<App />, document.getElementById('root'));