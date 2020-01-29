import { EmailGroup } from './models/email-group.model';
import { Component, TemplateRef, OnInit } from '@angular/core';
import { faCoffee, faPlus, faEdit, faTrash, faSpinner, faCircleNotch, faEnvelope } from '@fortawesome/free-solid-svg-icons';
import { BackendService } from './services/backend.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  public title = 'QuickMails';
  public isLoading = true;

  public faPlus = faPlus;
  public faEdit = faEdit;
  public faTrash = faTrash;
  public faSpinner = faCircleNotch;
  public faEnvelope = faEnvelope;

  public groups: EmailGroup[] = [];

  constructor(private backendService: BackendService) { }

  public async ngOnInit(): Promise<void> {
    this.isLoading = true;
    this.groups = await this.backendService.getGroups();
    this.isLoading = false;
  }

  public async onGrupAdded(newGroup: EmailGroup): Promise<void> {
    const addedGroup = await this.backendService.addGroups(newGroup);
    this.groups.push(addedGroup);
  }

  public async onGroupEdited(editedGroup: EmailGroup): Promise<void> {
    const groupIndex = this.groups.findIndex(item => item.id === editedGroup.id);
    if (groupIndex === -1) {
      return;
    }

    const updatedGroup = await this.backendService.updateGroup(editedGroup);
    this.groups[groupIndex] = updatedGroup;
  }

  public async deleteGroup(groupId: number): Promise<void> {
    const groupIndex = this.groups.findIndex(item => item.id === groupId);
    this.groups.splice(groupIndex, 1);

    await this.backendService.deleteGroup(groupId);
  }

}
