declare var require: any

var React = require('react');

import { ToolbarForm, IToolbarFormProps } from './ToolbarForm';

const serverAddr: string = "http://localhost:7071/api/rotate";

interface IState {
    rotateDeg: number;
    isCustom: boolean;
}

export default class RotateForm extends ToolbarForm<IToolbarFormProps, IState> {
    constructor(props, state) {
        super(props, state);

        this.state = {
            rotateDeg: 0
        };

        this.handleRotatePresetChange = this.handleRotatePresetChange.bind(this);
        this.handleRotateCustomChange = this.handleRotateCustomChange.bind(this);
        this.execute = this.execute.bind(this);
    }

    handleRotatePresetChange(event) {
        console.log("Value: " + event.target.value);
        if (event.target.value === "custom") {
            this.setState({ isCustom: true });
        } else {
            this.setState({ isCustom: false, rotateDeg: event.target.value });
        }
    }

    handleRotateCustomChange(event) {
        this.setState({ rotateDeg: event.target.value });
    }

    execute() {
        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = () => this.handleResponse(xhr);

        let content = JSON.stringify({
            "RotateByDeg": this.state.rotateDeg,
            "InputImage": this.props.inputImage
        });
        xhr.open("POST", serverAddr);
        xhr.setRequestHeader("Content-Type", "text/plain");
        xhr.send(content);
    }

    render() {
        return (
            <div className="action-form">
                <label htmlFor="rotate-preset">Rotate:</label>
                <select id="rotate-preset" name="rotate-preset" onChange={this.handleRotatePresetChange}>
                    <option value="90">90&deg;</option>
                    <option value="180">180&deg;</option>
                    <option value="270">270&deg;</option>
                    <option value="custom">Custom</option>
                </select>
                <input type="number" disabled={!this.state.isCustom} id="rotate-custom" name="rotate-custom" onChange={this.handleRotateCustomChange}></input>
                <label htmlFor="rotate-custom">&deg;</label>
                <button onClick={this.execute} disabled={!this.props.isEnabled}>Rotate</button>
            </div>
        );
    }
}