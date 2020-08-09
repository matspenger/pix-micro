import Memento from './Memento';

export default class Caretaker<T> {
    private history: Memento<T>[] = [];
    private rollbackIndex: number = 0;

    save(state: T): void {
        // New saves should clear old redos
        if (this.rollbackIndex > 0) {
            this.history = this.history.slice(0, this.history.length - this.rollbackIndex);
            this.rollbackIndex = 0;
        }
        this.history.push(new Memento<T>(state));
    }

    canUndo(): boolean {
        return (this.rollbackIndex + 1) < this.history.length;
    }

    undo(): T {
        if (this.canUndo()) {
            const index = this.history.length - (++this.rollbackIndex + 1);
            const memento = this.history[index];
            return memento.getState();
        }
    }

    canRedo(): boolean {
        return this.rollbackIndex > 0;
    }

    redo(): T {
        if (this.canRedo()) {
            const index = this.history.length - (--this.rollbackIndex + 1);
            const memento = this.history[index];
            return memento.getState();
        }
    }
}