import { Component, OnInit, Output, ViewChild, Input, EventEmitter, HostListener, ElementRef } from '@angular/core';
import { SupportedKeys } from '../helpers/supported-keys';
import { TooltipDirective } from 'ngx-bootstrap/tooltip/ngx-bootstrap-tooltip';

@Component({
    selector: 'app-inline-email-input',
    templateUrl: 'inline-email-input.component.html',
    styleUrls: ['inline-email-input.component.scss']
})
export class InlineEmailInputComponent {
    @Output()
    public emailChanged = new EventEmitter<string>();

    public inputEmail: string;

    @ViewChild('nativeEmailInput', { static: true })
    private emailInputElement: ElementRef;

    @ViewChild('emailTooltip', { static: true })
    private emailTooltip: TooltipDirective;

    private isEnabled = false;

    public apply() {
        if (!this.isEnabled) {
            return;
        }

        if (!this.emailInputElement.nativeElement.checkValidity()) {
            this.focusOnInput();
            this.emailTooltip.show();
            return;
        }

        this.isEnabled = false;
        this.emailChanged.emit(this.inputEmail);
        this.inputEmail = '';
    }

    public enable(initValue = '') {
        this.focusOnInput();
        this.isEnabled = true;
        this.inputEmail = initValue;
    }

    @HostListener('window:keyup', ['$event'])
    private keyEvent(event: KeyboardEvent) {
        if (!this.isEnabled) {
            return;
        }

        if (event.key === SupportedKeys.ENTER) {
            this.apply();
        }
    }

    private focusOnInput(): void {
        // Run on next cycle in case input just become visible
        setTimeout(() => this.emailInputElement.nativeElement.focus());
    }
}
