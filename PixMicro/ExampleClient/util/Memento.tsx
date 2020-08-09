export default class Memento<T> {
    private state: T;

    constructor(state: T) {
        this.state = state;
    }

    getState(): T {
        return this.state;
    }
}