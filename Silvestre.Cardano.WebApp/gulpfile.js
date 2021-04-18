/// <binding BeforeBuild='clean' AfterBuild='css' Clean='clean' ProjectOpened='watch' />
const gulp = require('gulp');

gulp.task('clean', () => {
    const clean = require('gulp-clean');

    return gulp.src('./wwwwroot/css/*.css', { read: false }).pipe(clean());
});

gulp.task('css', () => {
    const postcss = require('gulp-postcss');

    return gulp.src('./Content/layout.css')
        .pipe(postcss([
            require('precss'),
            require('tailwindcss'),
            require('autoprefixer')
        ]))
        .pipe(gulp.dest('./wwwroot/css/'));
});

gulp.task('watch', () => {
    return gulp.watch('./Pages/CSS/*.css', { delay: 500 }, gulp.series(['clean', 'css']));
});