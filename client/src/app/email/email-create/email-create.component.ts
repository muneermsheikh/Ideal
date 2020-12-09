
// import { InjectableCompiler } from '@angular/compiler/src/injectable_compiler';
// import { nullSafeIsEquivalent } from '@angular/compiler/src/output/output_ast';
import { Component, OnInit } from '@angular/core';
import { EmailValidator, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { IClient, IClientOfficial } from 'src/app/shared/models/client';
import { Email, IEmail } from 'src/app/shared/models/email';
import { IEnquiry } from 'src/app/shared/models/enquiry';

import { EmailService } from '../email.service';

@Component({
  selector: 'app-email-create',
  templateUrl: './email-create.component.html',
  styleUrls: ['./email-create.component.scss']
})
export class EmailCreateComponent implements OnInit {createForm: FormGroup;
  email: IEmail;
  customer: IClient;
  officials: IClientOfficial[];
  enquiry: IEnquiry;
  pageTitle: string;
  enquiryId: number;

  formErrors = {
   };

  validationMessages = {
      senderEmailAddress: {
        required: 'Sender email address is required',
        email : 'invalid email'
      },
      messageBody: {
        required: 'Message Body is required'
      },
      suject: {
        required: 'Subject is required',
      },
      toEmailList: {
        required: 'To email address is required'
      }
    };


  constructor(private fb: FormBuilder,
              private service: EmailService,
              private activatedRoute: ActivatedRoute,
              private route: Router)
  {
  }

  ngOnInit(): void {
    this.createForm = this.fb.group({
      id: [0],
      senderEmailAddress: [null, [Validators.required, Validators.email]],
      refNo: '',
      senderName: '',
      toEmailList: ['', [Validators.required, Validators.email]],
      ccEmailList: ['', Validators.email],
      bccEmailList: ['', Validators.email],
      dateSent: '',
      subject:  ['', Validators.required],
      messageType: '',
      messageBody: ['', Validators.required]
      });

    const enquiryId = +this.activatedRoute.snapshot.paramMap.get('id');

    if (enquiryId) {
        // insert
        this.enquiryId = enquiryId;
        this.pageTitle = 'new Message';
        this.email = new Email();
        this.service.getEnquiry(enquiryId).subscribe( enq => {
          this.enquiry = enq;
          this.service.getCustomerFromEnquiryId(this.enquiryId).subscribe(response => {
            this.customer = response;
            this.officials = response.customerOfficials;
            this.email = this.populateNewAckn(this.email, response, enq);
            console.log('returned frompopulateNewAckn');
            console.log(this.email);
          });
        });
    } else {
        // edit
        this.enquiryId = enquiryId;
        this.pageTitle = 'edit Messages';
        this.getDLAcknEmail(enquiryId);
    }

    this.editEmail(this.email);
    console.log('returned from patch value');
  }

    populateNewAckn(mail: IEmail, cust: IClient, enquiry: IEnquiry): IEmail{
      const msgBody = this.getMessageBody(cust, enquiry);
      console.log('msgBody');
      console.log(msgBody);
      mail.id = 0,
      mail.subject = 'Acknowledgement of equiry',
      mail.toEmailList = cust.email,
      mail.messageBody = msgBody;
      mail.messageType = 'DLAcknowledgement';

      return mail;
    }

    getMessageBody(customer: IClient, enq: IEnquiry): string {
      const officials = customer.customerOfficials;
      const enqItems = enq.enquiryItems;

      let title = '';
      let salute = '';
      let msg: string = new Date().getDate().toString();

      if (officials[0].gender.toLowerCase() === 'm') {
        title = 'Mr. ';
        salute = 'Sir: ';
      } else {
        title = 'Ms. ';
        salute = 'Madam: ';
      }
      msg += officials[0].name;
      if (officials[0].designation) {
        msg += ', ' + officials[0].designation;
      }

      if (officials[0].email) {msg += '<br>email: ' + officials[0].email; }

      msg += title + salute + '<br><br>Thank you for your enquiry dated ' + enq.enquiryDate +
        'for following items.  The enquiry has been assigned a reference number ' + enq.enquiryNo +
        ' by our system.: <br><br>Enquiry Items<br>';
      msg += '<table><tr><th>Sr.No.</th><th>Profession</th><th>Qnty</th><th>salary</th><th><remarks</th></tr>';

      enqItems.forEach(item =>
        {
          msg += '<tr><td>' + enq.enquiryNo + '-' + item.srNo + '</td><td>' + item.categoryName ?? '' +
          '</td><td>' + item.quantity + '</td><td>';
          if (item.remuneration === null)
          {
            msg += '</td>';
          } else {
            msg += item.remuneration.salaryCurrency ?? '' + ' ';
            msg += item.remuneration.salaryMin === 0 ? 'not known' : item.remuneration.salaryMin;
            msg += item.remuneration.salaryMax ?? '' + '</td>';
          }
          msg += '<td></td>';
        }
      );

      msg += '</tr></table><br><br>This is a system generated message and we will revert with our execution plan ' +
        'by our next message.<br><br>Best regards<br><br>';
      console.log(msg);
      return msg;
    }

    blankOutTheFields(): void {
        this.email = {
          id: 0,
          senderEmailAddress: '',
          refNo: '',
          senderName: null,
          toEmailList: null,
          ccEmailList: null,
          bccEmailList: null,
          dateSent: null,
          subject:  null,
          messageType: null,
          messageBody: null
      };
    }

    getDLAcknEmail(enquiryId: number): any {
      this.email = this.service.getDLAckEmail(enquiryId).subscribe(
        (mail: IEmail) => {
          if (mail) {
            this.email = mail;
          } else {
            this.email = new Email();
          }
          this.email = mail;
        },
        (error: any) => {this.email = new Email(); }
      );
    }

    editEmail(mail: IEmail): any {
      console.log('patching value');
      this.createForm.patchValue(
        {
          id: mail.id,
          senderEmailAddress: mail.senderEmailAddress,
          refNo: mail.refNo,
          senderName: mail.senderName,
          toEmailList: mail.toEmailList,
          ccEmailList: mail.ccEmailList,
          bccEmailList: mail.bccEmailList,
          dateSent: mail.dateSent,
          subject:  mail.subject,
          messageType: mail.messageType,
          messageBody: mail.messageBody
       });

    }

    onSubmit(): any {
      const mailVal = this.mapFormValuesToEmailObject();
      if (mailVal.id === null || mailVal.id === 0)    // INSERT mode
      {
        this.service.addEmail(mailVal).subscribe(() => {
          this.route.navigate(['email']);
        }, error => {
          console.log(error);
        });
      } else {                          // EDIT mode
        this.service.updateEmail(mailVal).subscribe(() => {
          this.route.navigate(['email']);
      }, error => {
        console.log(error);
      });
      }

    }

    mapFormValuesToEmailObject(): IEmail{
        this.email.id = this.createForm.value.id ;
        this.email.senderName = this.createForm.value.senderName;
        this.email.senderEmailAddress = this.createForm.value.senderEmailAddress;
        this.email.refNo = this.createForm.value.refNo;
        this.email.toEmailList = this.createForm.value.toEmailList;
        this.email.ccEmailList = this.createForm.value.ccEmailList;
        this.email.bccEmailList = this.createForm.value.bccEmailList;
        this.email.subject = this.createForm.value.subject;
        this.email.messageType = this.createForm.value.messageType;
        this.email.messageBody = this.createForm.value.messageBody;

        return this.email;
    }


    logValidationErrors(group: FormGroup = this.createForm): void {
      Object.keys(group.controls).forEach((key: string) => {
        const abstractControl = group.get(key);

        this.formErrors[key] = '';
        if (abstractControl && !abstractControl.valid &&
          (abstractControl.touched || abstractControl.dirty || abstractControl.value !== '')) {

            for (const errorKey in abstractControl.errors) {
              if (errorKey) {
                // this.formErrors[key] += messages[errorKey] + '';
              }
            }
          }

        if (abstractControl instanceof FormGroup) {
          this.logValidationErrors(abstractControl);
        }
      });
    }

}
