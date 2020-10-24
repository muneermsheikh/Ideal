import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ICandidate } from 'src/app/shared/models/ICand';
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
  form: FormGroup;
  errors: string[];
  idnumber: number;

  constructor(private cvService: UsersService, private fb: FormBuilder,
              private activatedRoute: ActivatedRoute, private router: Router,
              private bcService: BreadcrumbService) { }

  ngOnInit(): void {
    this.loadCandidate();
    this.getProfessions();
    this.createForm();
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

  createForm() {
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
      professionId: [null, [Validators.required]],
      ppNo:  [null],
      aadharNo:  [null] ,
      mobileNo:  [null, [Validators.required]],
      email:  [null, [Validators.email]],

      addressType:   [null, [Validators.required]],
      address1:   [null, [Validators.required]],
      address2: [null],
      city:   [null, [Validators.required]],
      pin:   [null, [Validators.required]],
      state:   [null],
      district: [null],
      country:   [null, [Validators.required]],
    });
  }


onSubmit() {
    console.log(this.form.value);
    this.cvService.updateCandidate(this.form.value).subscribe(response => {
      this.router.navigateByUrl('candidate');
    }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }}
