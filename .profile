alias ga='git add'
alias gaa='git add .'
alias gaaa='git add --all'
alias gb='git branch'
alias gba='git branch --all'
alias gbd='git branch -D'
alias gbmh='git branch -m $(git branch-name)'
alias gc='git commit'
alias gcm='git commit -m'
alias gca='git commit --amend'
alias gcano='git commit --amend --no-edit'
alias gd='git diff'
alias gdc='git diff --cached'
alias gdh='git diff HEAD~1 HEAD'
alias gdhs='git diff --stat HEAD~1 HEAD'
alias gds='git diff --stat'
alias gfo='git fetch origin'
alias gk='gitk --all'
alias gh='git log --date=format:"%Y-%m-%d %H:%M:%S" --graph --abbrev-commit --decorate --all --format=format:"%C(red)%h%C(reset)%C(auto)%d%C(reset) - %C(bold blue)%an%C(reset) - %C(cyan)%ad%C(reset)%n %C(white)%s%C(reset)"'
alias ghs='git log --graph --oneline --decorate --all'
alias gm='git merge'
alias gma='git merge --abort'
alias gmd='git merge develop'
alias gmnf='git merge --no-ff'
alias go='git checkout'
alias god='git checkout develop'
alias gom='git checkout master'
alias got='git checkout --track'
alias gph='git push'
alias gphd='git push develop'
alias gphm='git push master'
alias gpl='git pull'
alias gpld='git pull origin develop'
alias gplm='git pull origin master'
alias gr='git rebase'
alias gra='git rebase --abort'
alias gri='git rebase -i'
alias grih='git rih'
alias grc='git rebase --continue'
alias gs='git status --short --branch'
alias gst='git stash'
alias gsta='git stash apply'
alias gstc='git stash clear'
alias gstp='git stash pop'
alias gsts='git stash show'
alias gu='git reset HEAD'
alias wtf='git reset --hard head'
