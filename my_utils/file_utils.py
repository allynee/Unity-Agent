"""
File system utils
"""
import collections
import os
import sys
import shutil

def dump_text(s, *fpaths):
    full_path = f_join(*fpaths)
    os.makedirs(os.path.dirname(full_path), exist_ok=True)
    with open(full_path, "w") as fp:
        fp.write(s)

def f_join(*fpaths):
    """
    join file paths and expand special symbols like `~` for home dir
    """
    fpaths = pack_varargs(fpaths)
    fpath = f_expand(os.path.join(*fpaths))
    if isinstance(fpath, str):
        fpath = fpath.strip()
    return fpath

def f_expand(fpath):
    return os.path.expandvars(os.path.expanduser(fpath))

def pack_varargs(args):
    """
    Pack *args or a single list arg as list

    def f(*args):
        arg_list = pack_varargs(args)
        # arg_list is now packed as a list
    """
    assert isinstance(args, tuple), "please input the tuple `args` as in *args"
    if len(args) == 1 and is_sequence(args[0]):
        return args[0]
    else:
        return args
    
def is_sequence(obj):
    """
    Returns:
      True if the sequence is a collections.Sequence and not a string.
    """
    return isinstance(obj, collections.abc.Sequence) and not isinstance(obj, str)