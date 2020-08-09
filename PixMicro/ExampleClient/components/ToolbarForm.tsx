declare var require: any

var React = require('react');

const base64UriScheme = "data:image/jpeg;base64,";

export interface IToolbarFormProps {
    inputImage: string;
    outputHandler: (outputImg: string) => void;
    serverAddress: string;
    isEnabled: boolean;
}

export abstract class ToolbarForm<TProps extends IToolbarFormProps, TState> extends React.Component<TProps, TState> {
    constructor(props, state) {
        super(props, state);

        this.handleResponse = this.handleResponse.bind(this);
    }

    handleResponse(xhr: XMLHttpRequest) {
        if (xhr.readyState === 4 && xhr.status === 200) {
            let responseObj = JSON.parse(xhr.response);
            let base64 = responseObj["OutputImage"];
            if (!base64.startsWith(base64UriScheme)) {
                base64 = base64UriScheme + base64;
            }
            this.props.outputHandler(base64);
        }
    }
}