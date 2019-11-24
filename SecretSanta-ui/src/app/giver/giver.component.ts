import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({templateUrl: 'giver.component.html'})
export class GiverComponent implements OnInit {
    giftGiverForm: FormGroup;
    loading = false;
    submitted = false;
    returnUrl: string;
    error: string;
    recipient: any;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private http: HttpClient
    ) {
        // redirect to home if already logged in
        //if (this.authenticationService.currentUserValue) { 
        //    this.router.navigate(['/']);
        //}
    }

    ngOnInit() {
        this.giftGiverForm = this.formBuilder.group({
            giverName: ['', Validators.required]
        });

        // get return url from route parameters or default to '/'
        //this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    }

    // convenience getter for easy access to form fields
    get f() { return this.giftGiverForm.controls; }

    onSubmit() {
        debugger;
        this.submitted = true;

        // stop here if form is invalid
        if (this.giftGiverForm.invalid) {
            return;
        }

        this.loading = true;
        this.http.get<any>(`http://localhost:5000/api/SecretSanta/${this.f.giverName.value}`)
            .subscribe(data => {
                this.recipient = data
                this.loading = false;
            });
    }
}