import { initializeApp, getApps } from "https://www.gstatic.com/firebasejs/12.13.0/firebase-app.js";
import {
    browserLocalPersistence,
    confirmPasswordReset,
    getAuth,
    onIdTokenChanged,
    setPersistence,
    signInWithEmailAndPassword,
    signOut,
    verifyPasswordResetCode
} from "https://www.gstatic.com/firebasejs/12.13.0/firebase-auth.js";

let app;
let auth;
let authReadyPromise;

export async function initializeFirebaseAuth(options) {
    if (!app) {
        app = getApps().length > 0 ? getApps()[0] : initializeApp({
            apiKey: options.apiKey,
            authDomain: options.authDomain,
            projectId: options.projectId,
            storageBucket: options.storageBucket,
            messagingSenderId: options.messagingSenderId,
            appId: options.appId,
            measurementId: options.measurementId
        });

        auth = getAuth(app);
        auth.tenantId = options.tenantId;
        await setPersistence(auth, browserLocalPersistence);
    }

    authReadyPromise ??= new Promise(resolve => {
        const unsubscribe = onIdTokenChanged(auth, () => {
            unsubscribe();
            resolve();
        });
    });

    await authReadyPromise;
}

export async function signIn(email, password) {
    const credential = await signInWithEmailAndPassword(auth, email, password);
    return toAuthUser(credential.user, true);
}

export async function getCurrentUser(forceRefresh) {
    await authReadyPromise;

    if (!auth.currentUser)
        return null;

    return toAuthUser(auth.currentUser, forceRefresh);
}

export async function signOutUser() {
    await signOut(auth);
}

export async function verifyPasswordReset(oobCode) {
    await authReadyPromise;
    return await verifyPasswordResetCode(auth, oobCode);
}

export async function confirmPasswordResetCode(oobCode, newPassword) {
    await authReadyPromise;
    await confirmPasswordReset(auth, oobCode, newPassword);
}

async function toAuthUser(user, forceRefresh) {
    const idToken = await user.getIdToken(forceRefresh);

    return {
        uid: user.uid,
        email: user.email,
        displayName: user.displayName,
        idToken
    };
}
