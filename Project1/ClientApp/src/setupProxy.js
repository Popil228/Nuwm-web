const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:50651';

const context = [
    "/api/getschedule/date",
    "/api/setschedule/delete",
    "/api/setschedule/schedule",
    "/api/setschedule/subgroups_teachers",
    "/api/setschedule/subjects",
    "/api/setschedule/data",
    "/api/teacheraccount/delete-grade",
    "/api/teacheraccount/add-grade",
    "/api/teacheraccount/subjects",
    "/api/teacheraccount/students",
    "/api/teacheraccount/groups",
    "/api/teacheraccount/data",
    "/api/account/group-student-total-scores",
    "/api/account/student-total-scores",
    "/api/account/data",
    "/api/contact/contact",
    "/api/auth/login",
    "/_configuration",
    "/.well-known",
    "/Identity",
    "/connect",
    "/ApplyDatabaseMigrations",
    "/_framework"
];

module.exports = function (app) {
    const appProxy = createProxyMiddleware(context, {
        target: target,
        secure: false,
        headers: {
            Connection: 'Keep-Alive'
        }
    });

    app.use(appProxy);
};
