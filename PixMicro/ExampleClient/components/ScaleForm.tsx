declare var require: any

var React = require('react');

import { ToolbarForm, IToolbarFormProps } from './ToolbarForm';

const serverAddr: string = "http://localhost:7071/api/scale";

interface IState {
    mode: string,
    width: number,
    height: number,
    factor: number
}

export default class ScaleForm extends ToolbarForm<IToolbarFormProps, IState> {
    constructor(props, state) {
        super(props, state);

        this.state = {
            mode: "Factor",
            width: undefined,
            height: undefined,
            factor: undefined
        }

        this.isFactor = this.isFactor.bind(this);
        this.isPixels = this.isPixels.bind(this);
        this.handleModeChange = this.handleModeChange.bind(this);
        this.handleValueChange = this.handleValueChange.bind(this);
        this.execute = this.execute.bind(this);
    }

    isFactor() {
        return this.state.mode === "Factor";
    }

    isPixels() {
        return this.state.mode === "Pixels";
    }

    handleModeChange(event) {
        this.setState({ mode: event.target.value });
    }

    handleValueChange(event) {
        switch (event.target.name) {
            case "scale-px":
                this.setState({ width: event.target.value });
                break;
            case "scale-py":
                this.setState({ height: event.target.value });
                break;
            case "scale-factor":
                this.setState({ factor: event.target.value });
                break;
        }
    }

    execute() {
        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = () => this.handleResponse(xhr);
        let content;
        if (this.state.mode === "Factor") {
            content = JSON.stringify({
                "Mode": this.state.mode,
                "Factor": this.state.factor,
                "InputImage": this.props.inputImage
            });
        } else if (this.state.mode === "Pixels") {
            content = JSON.stringify({
                "Mode": this.state.mode,
                "Width": this.state.width,
                "Height": this.state.height,
                "InputImage": this.props.inputImage
            });
        }
        xhr.open("POST", serverAddr);
        xhr.setRequestHeader("Content-Type", "text/plain");
        xhr.send(content);
    }

    render() {
        return (
            <div className="action-form">
                <label htmlFor="scale-mode">Scale:</label>
                <select id="scale-mode" name="scale-mode" onChange={this.handleModeChange}>
                    <option value="Factor">Factor</option>
                    <option value="Pixels">Pixels</option>
                </select>
                <div id="factor-controls" className={this.isFactor() ? "visible" : "hidden"}>
                    <input type="number" step={0.1} id="scale-factor" name="scale-factor" onChange={this.handleValueChange}></input>
                    <label htmlFor="scale-factor">x</label>
                </div>
                <div id="pixels-controls" className={this.isPixels() ? "visible" : "hidden"}>
                    <input type="number" id="scale-px" name="scale-px" onChange={this.handleValueChange}></input>
                    <label>x</label>
                    <input type="number" id="scale-py" name="scale-py" onChange={this.handleValueChange}></input>
                </div>
                <button onClick={this.execute} disabled={!this.props.isEnabled}>Scale</button>
            </div>
        );
    }
}