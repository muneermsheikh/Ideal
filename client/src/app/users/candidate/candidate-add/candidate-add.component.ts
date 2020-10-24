import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { IProfession } from 'src/app/shared/models/profession';
import { UsersService } from '../../users.service';

@Component({
  selector: 'app-candidate-add',
  templateUrl: './candidate-add.component.html',
  styleUrls: ['./candidate-add.component.scss']
})

export class CandidateAddComponent implements OnInit {
    professions: IProfession[] = [];
    addForm: FormGroup;
    errors: string[];

    constructor(private cvService: UsersService, private fb: FormBuilder, private router: Router) { }

    ngOnInit(): void {
      this.createForm();
      this.getProfessions();
    }


    getProfessions(): any {
      this.cvService.getProfessions().subscribe(response =>
        {this.professions = response; },
        error => {console.log(error); } );
    }


  createForm() {
      this.addForm = this.fb.group({
      applicationDated:  [null, [Validators.required]],
      gender:  ['M', [Validators.required]],
      firstName:  [null, [Validators.required]],
      secondName:  [null],
      familyName:  [null],
      knownAs: [null, [Validators.required]],
      dOB: [null],
      eCNR: [false],
      professionId:  [null, [Validators.required]],
      ppNo:  [null],
      aadharNo:  [null] ,
      mobileNo:  [null, [Validators.required]],
      email:  [null, [Validators.email]],

      addressType:   ['R', [Validators.required]],
      address1:   [null, [Validators.required]],
      address2: [null],
      city:   [null, [Validators.required]],
      pin:   [null, [Validators.required]],
      state:   [null],
      district: [null],
      country:   ['India', [Validators.required]],
    });
  }


  onSubmit() {
      console.log(this.addForm.value);
      this.cvService.addCandidate(this.addForm.value).subscribe(response => {

      this.router.navigateByUrl('candidate');
    }, error => {
      console.log(error);
      this.errors = error.errors;
    });
  }

}
