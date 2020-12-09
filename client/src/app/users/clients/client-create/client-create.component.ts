import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IClient, IClientOfficial, ICustomerIndustryType } from 'src/app/shared/models/client';
import { IIndustryType } from 'src/app/shared/models/industryType';
import { ClientsService } from '../clients.service';

@Component({
  selector: 'app-client-create',
  templateUrl: './client-create.component.html',
  styleUrls: ['./client-create.component.scss']
})

export class ClientCreateComponent implements OnInit {

  form: FormGroup;
  client: IClient;
  pageTitle: string;

  indTypes: IIndustryType[];

  constructor(private fb: FormBuilder,
              private activatedRoute: ActivatedRoute,
              private router: Router,
              private service: ClientsService) { }

  ngOnInit(): void {

    this.getIndustryTypes();

    this.form = this.fb.group
      ({
        id: [0],
        customerType: ['customer', Validators.required],
        customerName: ['saudi bin zagr trading', Validators.required],
        knownAs: ['bin zagr', [Validators.required]],
        address1: ['po box 7296', [Validators.required]],
        address2: [null],
        city: ['dammam', Validators.required],
        pin: [null],
        state: [null],
        country: ['saudi arabia', Validators.required],
        introducedBy: [null],
        email: [null],
        mobile: [null],
        phone: [null],
        companyUrl: [null],
        description: [null],
        addedOn: '2020-01-01T00:00:00',
        customerStatus: [null],
        customerOfficials: this.fb.array([
          this.newCustomerOfficial()
        ]),
        customerIndustryTypes: this.fb.array([
          this.newIndustryType()
        ]),
      });

    const clientId = +this.activatedRoute.snapshot.paramMap.get('id');

    if (clientId) {
        // edit
        this.pageTitle = 'edit Client';
        this.getClient(clientId);
      } else {
        // insert
        this.pageTitle = 'new client';
        this.blankOutTheFields();
      }
  }

  blankOutTheFields(): void {
    this.client = {
      id: 0,
      customerType: '',
      customerName: '',
      knownAs: '',
      address1: '',
      address2: '',
      city: '',
      pin: '',
      state: '',
      country: '',
      introducedBy: '',
      mobile: '',
      email: '',
      phone: null,
      companyUrl: null,
      description: null,
      addedOn: null,
      customerStatus: null,
      customerOfficials: [],
      customerIndustryTypes: []
    };
  }


  getClient(id: number): any {
    this.client = this.service.getClient(id).subscribe(
      (client: IClient) => {
        this.editClient(client);
        this.client = client;
      },
      (error: any) => console.log('console error' + error)
    );
  }

  editClient(client: IClient): any {
    this.form.patchValue(
      {
        id: client.id,
        customerType: client.customerType,
        customerName: client.customerName,
        knownAs: client.knownAs,
        address1: client.address1,
        address2: client.address2,
        city: client.city,
        pin: client.pin,
        state: client.state,
        country: client.country,
        introducedBy: client.introducedBy,
        mobile: client.mobile,
        email: client.email,
        phone: client.phone,
        companyUrl: client.companyUrl,
        description: client.description,
        addedOn: client.addedOn,
        customerStatus: client.customerStatus
      });

    if (client.customerOfficials !== null) {
        this.form.setControl('customerOfficials', this.setExistingOfficials(client.customerOfficials));
      }

    if (client.customerIndustryTypes !== null) {
        this.form.setControl('customerIndustryTypes', this.setExistingIndustryTypes(client.customerIndustryTypes));
      }

    }

    customerOfficials(): FormArray {
      return this.form.get('customerOfficials') as FormArray;
    }

    customerIndustryTypes(): FormArray {
      return this.form.get('customerIndustryTypes') as FormArray;
    }

    setExistingOfficials(officials: IClientOfficial[]): FormArray {
      const formArray = new FormArray([]);
      officials.forEach( s => {
        formArray.push(this.fb.group( {
          id: s.id,
          customerId: s.customerId,
          name: s.name,
          designation: s.designation,
          gender: s.gender,
          phone: s.phone,
          mobile: s.mobile,
          mobile2: s.mobile2,
          email: s.email,
          personalEmail: s.personalEmail,
          personalMobile: s.personalMobile,
          addedOn: s.addedOn
          // isValid: s.isValid
          // name ignored, as using dropdown
        }));
      });
      return formArray;
    }

    setExistingIndustryTypes(customerIndustryTypes: ICustomerIndustryType[]): FormArray {
      const formArray = new FormArray([]);
      customerIndustryTypes.forEach( s => {
        formArray.push(this.fb.group( {
          id: s.id,
          industryTypeId: s.industryTypeId
          // name ignored, as using dropdown
        }));
      });
      return formArray;
    }

    newCustomerOfficial(): FormGroup{
      return this.fb.group({
          id: [0],
          customerId: [0],
          name: ['Abdullah Zain', Validators.required],
          designation: ['Personal officer'],
          gender: ['M', Validators.required],
          phone: ['394950044', Validators.required],
          mobile: [null],
          mobile2: [null],
          email: ['zain@gmail.com', [Validators.required, Validators.email]],
          personalEmail: [null],
          personalMobile: [null]
          // isValid: ['true']
      });
    }

    newIndustryType(): FormGroup{
      return this.fb.group({
          id: [0],
          industryTypeId: [5, Validators.required]
      });
    }

    pushNewCustomerOfficial(): void {
      this.customerOfficials().push(this.newCustomerOfficial());
    }

    pushNewIndustryType(): void {
      this.customerIndustryTypes().push(this.newIndustryType());
    }

    removeCustomerOfficial(i: number): any {
      const customerOfficials = this.form.get('customerOfficials');
      customerOfficials.markAsDirty();
      customerOfficials.markAsTouched();
      this.customerOfficials().removeAt(i);
    }

    removeIndustryType(i: number): any {
      const industryTypes = this.form.get('industryTypes');
      industryTypes.markAsDirty();
      industryTypes.markAsTouched();
      this.customerIndustryTypes().removeAt(i);
    }

    getIndustryTypes(): any {
      this.service.getIndustryTypes().subscribe(response => {
        this.indTypes = response;
      }, error => {
        console.log('error indu types' + error);
      });
    }

    onSubmit(): any {
      const clientVal = this.mapFormValuesToClientObject();
      console.log(clientVal.id);
      if (clientVal.id === null || clientVal.id === 0)    // INSERT mode
      {
        console.log(clientVal);
        this.service.addClient(clientVal).subscribe(() => {
          this.router.navigate(['/client']);
        }, error => {
          console.log(error);
        });
      } else {                          // EDIT mode
        this.service.updateClient(clientVal).subscribe(() => {
          this.router.navigate(['/client']);
      }, error => {
        console.log(error);
      });
      }
    }

    mapFormValuesToClientObject(): IClient{
        this.client.id = this.form.value.id ;
        this.client.customerType = this.form.value.customerType ?? 'customer';
        this.client.customerName = this.form.value.customerName;
        this.client.knownAs = this.form.value.knownAs;
        this.client.companyUrl = this.form.value.companyUrl;
        this.client.introducedBy = this.form.value.introducedBy;
        this.client.description = this.form.value.description;
        this.client.address1 = this.form.value.address1;
        this.client.address2 = this.form.value.address2;
        this.client.city = this.form.value.city;
        this.client.pin = this.form.value.pin;
        this.client.state = this.form.value.state;
        this.client.country = this.form.value.country ;
        this.client.phone = this.form.value.phone;
        this.client.email = this.form.value.email ;
        this.client.addedOn = this.form.value.addedOn;
        this.client.customerStatus = this.form.value.customerStatus;
        this.client.customerOfficials = this.form.value.customerOfficials;
        this.client.customerIndustryTypes = this.form.value.customerIndustryTypes;

        return this.client;
    }

}
