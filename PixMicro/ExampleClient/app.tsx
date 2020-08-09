declare var require: any

var React = require('react');
var ReactDOM = require('react-dom');

import FlipForm from './components/FlipForm';
import RotateForm from './components/RotateForm';
import ScaleForm from './components/ScaleForm';
import Caretaker from './util/Caretaker';

interface IProps {
}

interface IState {
    imageBase64: string;
    caretaker: Caretaker<string>;
}

export class App extends React.Component<IProps, IState> {

    constructor(props, state) {
        super(props, state);

        this.state = {
            imageBase64: "",
            caretaker: new Caretaker<string>()
        };

        this.state.caretaker.save("");

        this.loadImage = this.loadImage.bind(this);
        this.updateImage = this.updateImage.bind(this);
        this.hasImage = this.hasImage.bind(this);
        this.undo = this.undo.bind(this);
        this.redo = this.redo.bind(this);
    }

    loadImage(event): void {
        let fr = new FileReader();
        fr.addEventListener("load", () => {
            this.state.caretaker.save(fr.result);
            this.setState({ imageBase64: fr.result });
        })
        fr.readAsDataURL(event.target.files[0]);
    }

    hasImage(): boolean {
        return this.state.imageBase64 !== "";
    }

    updateImage(imageBase64: string) {
        this.setState({ imageBase64: imageBase64 });
    }

    undo() {
        this.updateImage(this.state.caretaker.undo());
    }

    redo() {
        this.updateImage(this.state.caretaker.redo());
    }

    render() {
        return (
            <div id="content">
                <div id="content-inner">
                    <div className="toolbar" id="main-toolbar">
                        <button disabled={!this.state.caretaker.canUndo()} onClick={this.undo}>Undo</button>
                        <button disabled={!this.state.caretaker.canRedo()} onClick={this.redo}>Redo</button>
                        <FlipForm outputHandler={this.updateImage} isEnabled={this.hasImage()} inputImage={this.state.imageBase64} caretaker={this.state.caretaker} />
                        <RotateForm outputHandler={this.updateImage} isEnabled={this.hasImage()} inputImage={this.state.imageBase64} caretaker={this.state.caretaker} />
                        <ScaleForm outputHandler={this.updateImage} isEnabled={this.hasImage()} inputImage={this.state.imageBase64} caretaker={this.state.caretaker} />
                    </div>
                    <div id="image">
                        <div id="file-form">
                            <label htmlFor="load-img">Load image file: </label>
                            <input type="file" id="load-img" name="load-img" onChange={this.loadImage} />
                        </div>
                        <div id="image-holder">
                            <div id="no-image-message" className={!this.hasImage() ? "visible" : "hidden"}>No image selected</div>
                            <img src={this.state.imageBase64} />
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}

ReactDOM.render(<App />, document.getElementById('root'));