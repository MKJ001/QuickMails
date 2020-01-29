import { EmailGroup } from './../models/email-group.model';
import { Component, Output, ViewChild, EventEmitter, TemplateRef } from '@angular/core';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { faTrash, faEdit } from '@fortawesome/free-solid-svg-icons';
import { Email } from '../models/email.model';
import { FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'app-new-group',
    templateUrl: 'new-group.component.html',
    styleUrls: ['new-group.component.scss']
})
export class NewGroupComponent {
    @Output()
    public gorupAdded = new EventEmitter<any>();

    @Output()
    public gorupEdited = new EventEmitter<any>();

    public title = '';
    public group: EmailGroup;
    public isNewMailActive = false;
    public nameControl: FormControl;

    public faTrash = faTrash;
    public faEdit = faEdit;

    @ViewChild('modalTemplate', { static: true })
    private modalTemplate: TemplateRef<any>;

    private modalRef: BsModalRef;
    private editMode = false;

    constructor(private modalService: BsModalService) {
        this.nameControl = new FormControl('', [Validators.required, Validators.maxLength(20)]);
    }

    public open(title: string, editMode = false, group = { emails: [] } as EmailGroup): void {
        this.title = title;
        this.editMode = editMode;

        this.nameControl.setValue(group.name);
        this.nameControl.markAsUntouched();

        this.group = Object.assign({}, group);
        this.group.emails = group.emails.map(item => item);

        this.modalRef = this.modalService.show(this.modalTemplate);
    }

    public close(): void {
        this.modalRef.hide();
    }

    public save(): void {
        if (!this.isValid()) {
            return;
        }

        this.group.name = this.nameControl.value;

        if (this.editMode) {
            this.gorupEdited.emit(this.group);
        } else {
            this.gorupAdded.emit(this.group);
        }

        this.close();
    }

    public addNewEmail(newEmail: string): void {
        this.isNewMailActive = false;

        if (newEmail.trim().length === 0) {
            return;
        }

        const email = { address: newEmail } as Email;
        this.group.emails.push(email);
    }

    public removeEmail(index: number): void {
        this.group.emails.splice(index, 1);
    }

    public updateEmail(email: Email, newValue: string): void {
        email.isEditMode = false;
        email.address = newValue;
    }

    private isValid(): boolean {
        this.nameControl.markAsTouched();

        return this.nameControl.valid;
    }
}
