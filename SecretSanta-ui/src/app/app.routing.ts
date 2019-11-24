import { Routes, RouterModule } from '@angular/router';
import { GiverComponent } from './giver';

const routes: Routes = [
    { path: '', component: GiverComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const appRoutingModule = RouterModule.forRoot(routes);