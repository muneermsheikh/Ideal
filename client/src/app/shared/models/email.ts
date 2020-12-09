export interface IEmail
{
    id: number;
    refNo: string;
    senderName: string;
    senderEmailAddress: string;
    toEmailList: string;
    ccEmailList: string;
    bccEmailList: string;
    dateSent: string;
    subject: string;
    messageType: string;
    messageBody: string;
}

export class Email implements IEmail{
    id: number;
    refNo: string;
    senderName: string;
    senderEmailAddress: string;
    toEmailList: string;
    ccEmailList: string;
    bccEmailList: string;
    dateSent: string;
    subject: string;
    messageType: string;
    messageBody: string;
}
