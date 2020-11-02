import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CandidateCategory, ICandidate } from 'src/app/shared/models/ICand';
import { IProfession } from 'src/app/shared/models/profession';
import { BreadcrumbService } from 'xng-breadcrumb';
import { UsersService } from '../../users.service';

@Component({
  selector: 'app-candidate-edit',
  templateUrl: './candidate-edit.component.html',
  styleUrls: ['./candidate-edit.component.scss']
})
export class CandidateEditComponent implements OnInit {

  candidate: ICandidate;
  professions: IProfession[] = [];
  candProfessions: CandidateCategory[] = [];
  form: FormGroup;
  candidateProfessions: FormArray;
  candidateAddresses: FormArray;
  errors: string[];
  idnumber: number;

  constructor(private cvService: UsersService, private fb: FormBuilder,
              private activatedRoute: ActivatedRoute, private router: Router,
              private bcService: BreadcrumbService) { }

  ngOnInit(): void {
    this.loadCandidate();
    this.getProfessions();
    this.createForm();
    // this.populateFormArrays();
  }


  loadCandidate(): void {

    this.idnumber =  +this.activatedRoute.snapshot.paramMap.get('id');
    if (isNaN(this.idnumber)) {this.idnumber = 12; }

    // this.idnumber = 7;
    this.cvService.getCandidate(this.idnumber).subscribe( candidate => {
      this.candidate = candidate;
      this.bcService.set('@candidatename', candidate.fullName);
      console.log('finishedcall from UsersService.getCandidate');
      console.log(this.candidate);
    }, error => {
      console.log(error);
    });
  }

  getProfessions(): any {
    this.cvService.getProfessions().subscribe(response =>
      {this.professions = response; },
      error => {console.log(error); } );
  }

  createForm(): void {
    this.form = this.fb.group({
      applicationNo: [null, [Validators.required]],
      applicationDated:  [null , [Validators.required]],
      gender:  [null, [Validators.required]],
      firstName:  [null, [Validators.required]],
      secondName:  [null],
      familyName:  [null],
      knownAs: [null, [Validators.required]],
      dOB: [null],
      eCNR: [null],
      ppNo:  [null],
      aadharNo:  [null] ,
      mobileNo:  [null, [Validators.required]],
      email:  [null, [Validators.email]],
      candidateProfessions: this.fb.array([this.createProfessionForm()]),
      candidateAddresses: this.fb.array([this.createAddressForm()])
    });
  }


  createAddressForm(): FormGroup {
    return this.fb.group({
      id: [null],
      addressType: [null, [Validators.required]],
      address1: [null],
      address2: [null],
      city: [null, [Validators.required]],
      pin: [null],
      state: [null],
      district: [null],
      country: [null]
      });
    }

    get AddressControls(): FormArray {
      return this.form.get('candidateAddresses') as FormArray;  // ['controls'];
    }


    addAddress(): void {
      this.candidateAddresses = this.form.get('candidateAddresses') as FormArray;
      this.candidateAddresses.push(this.createAddressForm());
    }

    removeAddress(i: number): void {
      this.candidateAddresses.removeAt(i);
    }

    logAddressValue(): void {
      console.log(this.candidateAddresses.value);
    }

    createProfessionForm(): FormGroup {
      return this.fb.group({
        id: [null, [Validators.required]],
        name: [null, [Validators.required]]
      });
   }

    get CategoryControls(): FormArray {
    // return this.form.get('categoryFormArray')['controls'];
    return this.form.get('candidateProfessions') as FormArray; // ['controls'];
  }

    addProfession(): void {
      this.candidateProfessions = this.form.get('candidateProfessions') as FormArray;
      this.candidateProfessions.push(this.createProfessionForm());
    }

    removeProfession(i: number): void {
      this.candidateProfessions.removeAt(i);
    }

onSubmit(): void {
    console.log(this.form.value);
    this.cvService.updateCandidate(this.form.value).subscribe(response => {
      this.router.navigateByUrl('candidate');
    }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }}
