declare var require: any

var React = require('react');

import { ToolbarForm, IToolbarFormProps } from './ToolbarForm';

const serverAddr: string = "http://localhost:7071/api/flip";

interface IState {
    flipMode: string;
}

export default class FlipForm extends ToolbarForm<IToolbarFormProps, IState> {

    constructor(props, state) {
        super(props, state);

        this.state = {
            flipMode: "X"
        };

        this.handleChange = this.handleChange.bind(this);
        this.execute = this.execute.bind(this);
    }

    handleChange(event) {
        this.setState({ flipMode: event.target.value });
    }

    execute() {
        let xhr = new XMLHttpRequest();
        xhr.onreadystatechange = () => this.handleResponse(xhr);

        let content = JSON.stringify({
            "Mode": this.state.flipMode,
            "InputImage": this.props.inputImage
        });
        xhr.open("POST", serverAddr);
        xhr.setRequestHeader("Content-Type", "text/plain");
        xhr.send(content);
    }

    render() {
        return (
            <div className="action-form">
                <label htmlFor="flip-mode">Flip:</label>
                <select id="flip-mode" name="flip-mode" onChange={this.handleChange}>
                    <option value="X">Horizontal</option>
                    <option value="Y">Vertical</option>
                    <option value="XY">Both axes</option>
                </select>
                <button onClick={this.execute} disabled={!this.props.isEnabled}>Flip</button>
            </div>
        );
    }
}