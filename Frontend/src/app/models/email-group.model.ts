import { Email } from './email.model';

export interface EmailGroup {
    id: number;
    name: string;
    emails: Email[];
    mailto: string;
}
